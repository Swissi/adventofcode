raw_ids = File.readlines('input.txt')
ids = []

raw_ids.each do |id|
  ids.push(id.split(''))
end

ids.each do |v|
  ids.each do |x|
    diff = v - x

    next unless diff.length == 1

    count = 0
    last_index = 0

    x.each_with_index do |item, i |
      if item != v[i]
        last_index = i
        count += 1
      end
    end

    if count == 1
      x.delete_at(last_index)
      puts x.join('')
    end
  end
end
