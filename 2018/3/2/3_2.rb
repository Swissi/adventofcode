#lines = File.readlines('input_example.txt')
lines = File.readlines('input.txt')
cloth = {}
claims = 0

lines.each do |line|
  #1 @ 669,271: 17x11
  arr = line.split(' ')
  coord = arr[2]
  area = arr[3]

  arr1 = coord.split(',')
  x = Integer(arr1[0])
  y = Integer(arr1[1].chomp(':'))

  arr2 = area.split('x')
  w = Integer(arr2[0])
  l = Integer(arr2[1])

  (1..w).each do |w|
    (1..l).each do |l|
      hash_key = (x + w).to_s + '_' + (y + l).to_s

      if cloth[hash_key].nil?
        cloth[hash_key] = 1
      elsif cloth[hash_key] == 1
        claims += 1
        cloth[hash_key] += 1
      else
        cloth[hash_key] += 1
      end
    end
  end
end

lines.each do |line|

  #1 @ 669,271: 17x11
  arr = line.split(' ')
  id = arr[0]
  coord = arr[2]
  area = arr[3]
  does_intersect = false

  arr1 = coord.split(',')
  x = Integer(arr1[0])
  y = Integer(arr1[1].chomp(':'))

  arr2 = area.split('x')
  w = Integer(arr2[0])
  l = Integer(arr2[1])

  (1..w).each do |w|
    (1..l).each do |l|
      hash_key = (x + w).to_s + '_' + (y + l).to_s
      if cloth[hash_key] > 1
        does_intersect = true
        break
      end
    end
  end

  puts id if does_intersect == false
end


