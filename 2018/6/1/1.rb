def get_manhattan_distance(x1, y1, x2, y2)
  # d((1,5),(6,1) = | 1 - 6 | + | 5 - 1 | = 9
  a = (x1 - x2).abs
  b = (y1 - y2).abs
  a + b
end

lines = File.readlines('i.txt')
coords = []
min_x = 10_000
max_x = 0
min_y = 10_000
max_y = 0


lines.each_with_index do |line,i|
  arr = line.split(',')
  x = Integer(arr[0])
  y = Integer(arr[1])

  min_x = x - 1 if x < min_x

  max_x = x + 1 if x > max_x

  min_y = y - 1 if y < min_y

  max_y = y if y > max_y

  coords[i] = [x,y]
end

short_distance_counter = {}
infinite_index = []
ties = 0

(min_y..max_y).each do |y|
  (min_x..max_x).each do |x|
    distances = {}
    coords.each_with_index do |c, i|
      d = get_manhattan_distance(x, y, c[0], c[1])
      distances[i] = d
    end

    sorted_distances = distances.sort_by{ |_, y| y }

    first = sorted_distances[0][1]
    second = sorted_distances[1][1]

    if first == second
      # no clear candidate found
      ties += 1
      print '..'
    else
      id = sorted_distances[0][0]

      # update ranking
      if short_distance_counter[id].nil?
        short_distance_counter[id] = 1
      else
        short_distance_counter[id] += 1
      end

      if x == min_x || x == max_x || y == min_y || y == max_y
        infinite_index.push(id)
      end

      if first.zero?
        print 'XX'
      else
        print sorted_distances[0][0]
      end
    end
  end
  puts ''
end

puts "we have #{ties} ties"

infinite_index.uniq.each do |i|
  short_distance_counter.delete(i)
  puts "deleting #{i} because its infinite"
end

first = short_distance_counter.sort_by{ |_, y| y }.reverse.first

puts first


puts 'end of code'
